using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ACNHWorldMVC.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Text { get; set; }

        [DisplayName("Author")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
