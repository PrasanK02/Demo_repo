using System;
using System.Collections;

namespace Cegedim.Automation {

    public class Customer {
        public string CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string DisplayName { get; set; }
//        public static Hashtable InitialIdentityValue() {
//            Hashtable identity = new Hashtable();
//            identity.Add("Last Name") = "Abbott";
//            identity.Add("First Name") = "Lisa";
//            identity.Add("Second Last Name") = "2ndlast";
//            identity.Add("Salutation") = "Yo";
//            identity.Add("Suffix") = "Jr";
//            identity.Add("Prof. Suffix") = "PA";
//            identity.Add("Prof. Title") = "Osteopath";
//            identity.Add() = "";
//            identity.Add() = "";
//            identity.Add() = "";
//            identity.Add() = "";
//            identity.Add() = "";
//            identity.Add() = "";
//        }
    }
}

//@cdl_name = "identity.cdl"
//    9     @cust_identity_fields = Hash[:lastname            => Array.[]("Last Name", "Abbott", :textentry),\
//        8                                 :firstname            => Array.[]("First Name", "Lisa", :textentry),\
//        7                                 :secondlastname       => Array.[]("Second Last Name", '2ndlast', :textentry),\
//        6                                 :salutation           => Array.[]("Salutation", 'Yo', :textentry),\
//        5                                 :suffix               => Array.[]("Suffix", 'Jr', :listpopup),\
//        4                                 :profsuffix           => Array.[]("Prof. Suffix", 'PA', :listpopup),\
//        3                                 :proftitle            => Array.[]("Prof. Title", 'Osteopath', :listpopup),\
//        2                                 :specialty            => Array.[]("Specialty", 'Obstetrics And Gynecology', :listpopup),\
//        1                                 :repspecialty         => Array.[]("Rep. Specialty", 'Allergy', :listpopup),\
//        0                                 :emailaddress         => Array.[]("Email Address", 'me@here.com', :textentry),\
//        1                                 :salesdatarstriction  => Array.[]("Sales Data Restriction", 'No', :textverify),\
//        2                                 :callcycleweekno      => Array.[]("Call Cycle Week Number", '1', :listpopup),\
//        3                                 :callcycleweekday     => Array.[]("Call Cycle Week Day", 'Tuesday', :listpopup),\
//        4                                 :practicesize         => Array.[]("Practice Size", '0 - 10', :listpopup),\
//        5                                 :birthyear            => Array.[]("Birth Year", '1962', :textentry),\
//        6                                 :birthmonth           => Array.[]("Birth Month", 'March', :listpopup),\
//        7                                 :birthday             => Array.[]("Birth Day", '12', :textentry),\
//        8                                 :visit                => Array.[]("Visit", true, :checkbox),\
//        9                                 :call                 => Array.[]("Call", true, :checkbox),\
//        10                                 :fax                  => Array.[]("Fax", true, :checkbox),\
//        11                                 :mail                 => Array.[]("Mail", true, :checkbox),\
//        12                                 :email                => Array.[]("Email", true, :checkbox)]
//