namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        [Display(Name="Código")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [StringLength(50)]
        public string Description { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "Disponible")]
        public bool IsAvailable  { get; set; }

        [Display(Name = "Publicado")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noproduct.png";
                }
                return $"https://salesapikarlos.azurewebsites.net{this.ImagePath.Substring(1)}";
                //return $"https://salesbackendkarlos.azurewebsites.net/{this.ImagePath.Substring(1)}";
            }
        }
    }
}
