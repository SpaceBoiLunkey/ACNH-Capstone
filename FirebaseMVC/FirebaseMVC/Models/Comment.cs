using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Post")]
        public int MessageId { get; set; }

        [DisplayName("Author")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Text { get; set; }
        public string Name { get; set; }
    }
}
