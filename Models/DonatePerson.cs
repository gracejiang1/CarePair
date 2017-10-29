using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finaldisaster.Models
{
    public class DonatePerson
    {
        public String name;
        public String phnum;
        public String material;
        public int amt;
        public double latitude;
        public double longitude;
        public String descrip;

        public DonatePerson()
        {
            this.name = null;
            this.amt = 0;
            this.phnum = null;
            this.material = null;
            this.latitude = 0;
            this.longitude = 0;
            this.descrip = null;

        }



    }

    

}