using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using analyzer;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("[controller]")]
    public class Compile : Controller
    {
        private readonly ILogger<Compile> _logger;


        public Compile(ILogger<Compile> logger)
        {
            _logger = logger;
        }

        public class CompileRequest
        {
            [Required]
            public required string code { get; set; }
        }

        // POST /compile
        [HttpPost]
        public IActionResult Post([FromBody] CompileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid request" });
            }

            var inputStream = new AntlrInputStream(request.code);
            var lexer = new LanguageLexer(inputStream);

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new DescriptiveErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new LanguageParser(tokens);

            // Agregar el listener de errores al lexer
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new CustomErrorListener());

            try
            {
                var tree = parser.program();
                var interpreter = new InterpreterVisitor();
                interpreter.Visit(tree);

                var compiler = new CompilerVisitor();
                compiler.Visit(tree);

                return Ok(new { result = compiler.c.ToString() });
            }
            catch (ParseCanceledException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (SemanticError ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("ast")]
        public async Task<IActionResult> GetAST([FromBody] CompileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid request" });
            }

            string grammarPath = Path.Combine(Directory.GetCurrentDirectory(), "Language.g4");
            // print current directory
            Console.WriteLine(Directory.GetCurrentDirectory());

            string grammar = "";

            try
            {
                if (System.IO.File.Exists(grammarPath))
                {
                    grammar = await System.IO.File.ReadAllTextAsync(grammarPath);
                }
                else
                {
                    return NotFound(new { error = "Grammar file not found", path = grammarPath });
                }

            }
            catch (IOException ex)
            {
                return StatusCode(500, new { error = "Error reading grammar files", details = ex.Message });
            }


            var payload = new
            {
                grammar,
                lexgrammar = "",
                input = request.code,
                start = "program"
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync("http://lab.antlr.org/parse/", content);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();

                    // Deserializa el JSON en un objeto dinámico
                    using var doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    // Extrae la propiedad "svgtree"
                    if (root.TryGetProperty("result", out JsonElement resultElement) &&
                        resultElement.TryGetProperty("svgtree", out JsonElement svgtreeElement))
                    {
                        string svgtree = svgtreeElement.GetString() ?? string.Empty;
                        return Content(svgtree, "image/svg+xml");

                    }

                    return BadRequest(new { error = "svgtree not found in response" });

                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, new { error = "Error making request", details = ex.Message });
                }
            }
        }

    }
}