using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJewelry.Models
{
    [Table("Cost")]
    public partial class AssemblyCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int companyId { get; set; }
        public string costDataJSON { get; set; }
    }

    // Handle binding - 
    //http://www.hanselman.com/blog/ASPNETWireFormatForModelBindingToArraysListsCollectionsDictionaries.aspx
    //https://stackoverflow.com/questions/1300642/asp-mvc-net-how-to-bind-keyvaluepair?noredirect=1&lq=1, 
    //https://stackoverflow.com/questions/23435219/binding-mvc-view-to-ienumerablekeyvaluepairstring-bool-issues
}