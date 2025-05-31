function CustomInputPoup(funcationName, Title, Text, Type, inputPalceholder) {
    if (Title != null && Title != "" && Text != null && Text != "" && Type == "input" && inputPalceholder != null && inputPalceholder != "" && funcationName != null && funcationName != "" ) {
        swal({
            title: Title,
            text: Text,
            type: Type,
            showCancelButton: true,
            closeOnConfirm: false,
            animation: "slide-from-top",
            inputPlaceholder: inputPalceholder
        }, function (inputValue) {
            if (inputValue === null) {
                return false;
            }

            if (inputValue === "") {
                swal.showInputError("You need to " + inputPalceholder);
                return false
            }
            if (inputValue === false) {
            }
            else {
            funcationName(inputValue);
            }
        });
    }
}

function CustomSwalPoup(Title, Text, Type) {
    if (Title != null && Title != "" && Text != null && Text != "" && Type != null && Type != "") {
        swal({
            html: true,
            title: Title,
            text: Text,
            type: Type
        });
    }
}

function CustomDeletePoup(Id, funcationName, Title, Text, Type) {
    if (Title != null && Title != "" && Text != null && Text != "" && Type == "warning" && Id != 0 && funcationName != null && funcationName != "" ) {
        swal({
            title: Title,
            text: Text,
            type: Type,
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'Yes',
            cancelButtonText: "No"
        }, function (isConfirm) {
            if (!isConfirm) return;
            funcationName(Id);
        });
    }
}
