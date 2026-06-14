namespace EmployeeManagement.Api.Pages.Shared;

public sealed record ConfirmationModalModel(
    string ModalId, // delete[Entity]Modal
    string Title,
    string Message,
    string Handler,
    string ConfirmButtonText = "Confirm",
    string ConfirmButtonClass = "btn-danger");
