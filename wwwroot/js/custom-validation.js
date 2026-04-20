// Add custom validation method to jQuery validation for the 'validflightdate' attribute
$.validator.addMethod("validflightdate", function (value, element) {
    if (!value) return false;

    var inputDate = new Date(value);
    
    // Reset time to midnight for an accurate day comparison
    var today = new Date();
    today.setHours(0,0,0,0);
    
    // Calculate 3 years from today
    var maxDate = new Date();
    maxDate.setFullYear(today.getFullYear() + 3);
    maxDate.setHours(0,0,0,0);

    // Ensure the date is strictly in the future and less than 3 years out
    return inputDate > today && inputDate <= maxDate;
});

// Wire up the unobtrusive adapter so ASP.NET Core recognizes it
$.validator.unobtrusive.adapters.addBool("validflightdate");