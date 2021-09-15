var messages= {
    required: "Please Input.",
        remote: "Please fix this field.",
        email: "Please enter a valid email address.",
        url:"Please enter a valid URL.",
        date: "Please enter a valid date.",
        dateISO: "Please enter a valid date ( ISO ).",
        number: "Please enter a valid number.",
        digits: "Please enter only digits.",
        creditcard: "Please enter a valid credit card number.",
        equalTo: "Please enter the same value again.",
        maxlength: $.validator.format( "Please enter no more than {0} characters." ),
        minlength: $.validator.format( "Please enter at least {0} characters." ),
        rangelength: $.validator.format( "Please enter a value between {0} and {1} characters long." ),
        range: $.validator.format( "Please enter a value between {0} and {1}." ),
        max: $.validator.format( "Please enter a value less than or equal to {0}." ),
        min: $.validator.format( "Please enter a value greater than or equal to {0}." )
}

jQuery.extend(jQuery.validator.messages, messages);

jQuery.validator.addMethod("isIdNo", function (value, element, param) {
    var idtype = jQuery(param).val();
    return this.optional(element) || checkFinUin(idtype,value);
}, "Please enter a valid ID Number.");
jQuery.validator.addMethod("isPhone", function (value, element) {
    return this.optional(element) || (isHandphoneNumberRegister(value));
}, "Please enter a valid phone number.");

jQuery.validator.addMethod("compareDate", function (value, element, param) {

    var startDate = jQuery(param[0]).val();
    var valiType = $("input[name='" + param[1] + "']:checked").val();
    var date1 = moment(startDate, "DD/MM/YYYY HH:mm");
    var date2 = moment(value, "DD/MM/YYYY HH:mm");  
    return valiType =="S" || this.optional(element) || date1 <= date2;
}, "The end date must be greater or equal to the start date");

jQuery.validator.addMethod("compareDate2", function (value, element, param) {
   // debugger;
    var startDate = jQuery(param[0]).val();
    var endDate = jQuery(param[1]).val();
    //var valiType = $("input[name='" + param[1] + "']:checked").val();
    var date1 = moment(startDate, "DD/MM/YYYY HH:mm");
    var date2 = moment(endDate, "DD/MM/YYYY HH:mm");
    var ret = date1 <= date2;
    console.log(date1 + "<=" + date2);
    return ret;
}, "The end date must be greater or equal to the start date");

jQuery.validator.addMethod("isDate", function (value, element) {

    return this.optional(element) || moment(value, "DD/MM/YYYY HH:mm").isValid();
}, "Please enter a valid date.");

jQuery.validator.addMethod("isOneDay", function (value, element, params) { 
   
    if (params.length > 2) {        
        var idType = params[2];
        if (idType == "Without ID") {             
            return true;
        }
    }
    
    var startDate = jQuery(params[0]).val();
    var valiType = $("input[name='" + params[1] + "']:checked").val(); 
    var date1 = moment(startDate, "DD/MM/YYYY HH:mm").format("DDMMYYYY");
    var date2 = moment(value, "DD/MM/YYYY HH:mm").format("DDMMYYYY");
    
    return valiType =="M" || date1 == date2;
}, "The end date must be same to the start date");
 
