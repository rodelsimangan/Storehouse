using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Storehouse.Model
{
    public class HomePageSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string TenantId { get; set; }

        public string HeaderLogo { get; set; }
        public string FooterLogo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyAddress { get; set; }
        public int SliderSpeed { get; set; }
        public string AboutCompany { get; set; }
        public string FAQsandTC { get; set; }
        public string EventsandPromos { get; set; }
        public string ContactUsEmailHost { get; set; }
        public string ContactUsEmailPort { get; set; }
        public string ContactUsEmailRecipient { get; set; }

    }
}