using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Enums;

namespace WebApi.ViewModels
{
    public class LikeViewModel
    {
        public long ObjectId { get; set; }
        [EnumDataType(typeof(Vote))]
        public Vote Vote { get; set; }
    }
}