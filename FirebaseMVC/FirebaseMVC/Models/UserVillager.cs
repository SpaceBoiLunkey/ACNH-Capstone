using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Models
{
    public class UserVillager
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int VillagerId { get; set; }
        public Villager Villager { get; set; }
        public UserVillager UserVillagers { get; set; }

    }
}