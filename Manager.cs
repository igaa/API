using System; 
using System.Text;
using System.Collections.Generic;
using System.Linq; 
using API.Models.Auth; 
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims; 
public class Manager : IManager
{
    private List<user> lst_user = new List<user>(); 

    private void get_dummy(){
        lst_user.Add(
            new user {
                Id = 1, 
                Name = "Igga", 
                Surname = "Igga  Fauzi Rahman", 
                UserName = "Irhmn", 
                Password = "pwd"
            } 
        ); 

        lst_user.Add( 
             new user {
                Id = 1, 
                Name = "Mamat", 
                Surname = "Muhammad Abdul Geni", 
                UserName = "iag", 
                Password = "pwd2"
            }
        ); 
    }
    public string Authtenticate(string username, string password)
    {

        this.get_dummy(); 

        var user = lst_user.Where(s => s.UserName.Equals(username) && s.Password.Equals(password)).ToList(); 

        if(user ==  null)
        return null; 

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes("This is the token do you like it"); 

        var tokenDescriptor = new SecurityTokenDescriptor{
            
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, user.FirstOrDefault().Id.ToString())
            } ), 
            Expires = DateTime.UtcNow.AddMinutes(5), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
            )
        }; 

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var result = tokenHandler.WriteToken(token); 

        return result.ToString(); 
    }
}