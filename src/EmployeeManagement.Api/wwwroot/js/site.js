document.querySelectorAll("[data-confirmation-modal]").forEach(modal => {
    modal.addEventListener("show.bs.modal", event => {
        const trigger = event.relatedTarget;

        modal.querySelector("[data-confirmation-id]").value =
            trigger.dataset.confirmationId;

        modal.querySelector("[data-confirmation-name]").textContent =
            trigger.dataset.confirmationName;
    });
});
