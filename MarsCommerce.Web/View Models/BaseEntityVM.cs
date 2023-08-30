using System.ComponentModel.DataAnnotations;
namespace MarsCommerce.Web.View_Models
{
    public class BaseEntityVM
    {
        [Key]
        public int Id { get; set; }
    }
}
