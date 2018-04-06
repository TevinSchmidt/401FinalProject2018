// To be completed by students in milestone 2

function validatecredentialForm() {
    if (credentialForm.username.value === "" || credentialForm.password.value === "") {
        return false;
    } else {
        return true;
    }

}

function validateCreationForm() {

    if (accountCreationForm.username.value === "" || accountCreationForm.email.value === ""
        || accountCreationForm.password.value === "" || accountCreationForm.confirmPassword.value === ""
        || accountCreationForm.address.value === "" || accountCreationForm.users.value === "Select Type"
        || accountCreationForm.phonenumber.value === "") {
		window.alert("Must Specify all Boxes.");
        return false;
    }
    if (accountCreationForm.password.value !== accountCreationForm.confirmPassword.value) {
		window.alert("Passwords Do Not Match");
        return false;
    }
    var email = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!email.test(String(accountCreationForm.email.value).toLowerCase())) {
        window.alert("Invalid E-mail. Try Again!");

        return false;
    }
    var phone = /^\d{3}-?\d{4}$/;
    if (!phone.test(String(accountCreationForm.phonenumber.value).toLowerCase())) {
        window.alert("Invalid Phone Number. Format should be xxx-xxxx");

        return false;
    }
    return true;
}