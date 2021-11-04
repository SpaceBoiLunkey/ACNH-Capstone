using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Models.ViewModels
{
    public class MessageDetailViewModel
    {
        public Message Message { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
