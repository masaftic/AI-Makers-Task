$.validator.addMethod("domainemail", function (value, element) {
    return this.optional(element) ||
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value.trim());
});

$.validator.unobtrusive.adapters.addBool("domain-email", "domainemail");

$.validator.addMethod("domainmobile", function (value, element) {
    const normalized = value.trim().replaceAll(" ", "").replaceAll("-", "");

    return this.optional(element) || /^\+[0-9]{7,15}$/.test(normalized);
});

$.validator.unobtrusive.adapters.addBool("domain-mobile", "domainmobile");
