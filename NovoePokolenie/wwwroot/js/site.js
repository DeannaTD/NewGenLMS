function startWait() {
    let waiter = document.createElement('div');
    waiter.className = "waiter-block";
    waiter.id = "waiter";
    waiter.appendChild(document.createElement('div'));
    document.body.appendChild(waiter);
}

function endWait() {
    let waiter = document.getElementById("waiter");
    waiter.remove();
}

function toCopy(element) {
    let text = element.innerHTML;
    navigator.clipboard.writeText(text);
    $(element).attr("style", "color: #F6BB7D;");
    setTimeout(() => {
        $(element).attr("style", "");
    }, 300);
}

function loadProjects(levelSelect) {
    $('#levelForm').submit();
}
