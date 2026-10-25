using Asp.Versioning;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(Roles = "1")]
public class UserController : ControllerBase
{
    private readonly BibliotecaContext _context;
    private readonly IConfiguration _config;

    public UserController(BibliotecaContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    // PUT: api/User/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            return BadRequest(new { mensaje = "El email ya está registrado" });

        // Encriptamos el password a utilizar
        string pswHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
        usuario.PasswordHash = pswHash;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Usuario creado con éxito!" });
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string pass)
    {
        var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        // verificamos si la contraseña es válida
        bool pwdValid = BCrypt.Net.BCrypt.Verify(pass, user.PasswordHash);

        if (!pwdValid)
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        var token = GenerarToken(user);
        return Ok(new { token });
    }

    private string GenerarToken(Usuario user)
    {
        // incorporamos los datos reales del usuario en los claim
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("nombre", user.NombreCompleto),
            new Claim(ClaimTypes.Role, user.RolId.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = creds
        };
        var tokenHadler = new JwtSecurityTokenHandler();
        var token = tokenHadler.CreateToken(tokenDescription);
        return tokenHadler.WriteToken(token);
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}