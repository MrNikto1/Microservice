using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private static List<User> _users = new()
    {
        new User { Id = 1, Name = "John Doe", Email = "john@example.com" }
    };

    [HttpGet]
    public IEnumerable<User> GetAll() => _users;

    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        user.Id = _users.Max(u => u.Id) + 1;
        _users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        _users.Remove(user);
        return NoContent();
    }
}