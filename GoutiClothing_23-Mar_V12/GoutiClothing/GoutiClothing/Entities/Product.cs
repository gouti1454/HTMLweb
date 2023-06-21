using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GoutiClothing.Entities
{
    //new class file is created and used for creating the model - "Product"
    //All the Columns of the table are created here.
    //example 3 properties for 3 columns
    //setting primary key and by default the below line will -"Id" will be passed on as primary key. 
    //if any other columns are to be added as primary key they need to be specified. 
    //type prop then press tab twice

    public class Product
    {
        //*************//
        //server side validation example =[Range(1, 100, ErrorMessage = "B/w 1 - 100")]
        //attributes are used to help for validation 
        // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-6.0

        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public int Price { get; set; }

        //ValidateNever is used such that the CRUD opertion exception- is the "model valid" condition is passed 
        [ValidateNever]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
