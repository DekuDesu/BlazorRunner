
function toggle_modal(name, value) {
    var myModal = new bootstrap.Modal(document.getElementById(name));

    console.log("Ran Modal");

    if (value == true) {
        myModal.show();
        return;
    }
    myModal.hide();
    return;
}