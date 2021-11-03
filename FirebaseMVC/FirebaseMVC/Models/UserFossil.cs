using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Models
{
    public class UserFossil
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int FossilId { get; set; }
        public Fossil Fossil { get; set; }
        public UserFossil UserFossils { get; set; }
        
    }
}