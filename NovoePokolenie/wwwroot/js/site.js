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


//-------------------------------------------------
//MARCH 2023
//-------------------------------------------------

//_Layout scripts
function getDateString(datetime) {
    let result = '';
    result += daysOfWeek[datetime.getDay()] + ', ';
    result += getTwoDigit(datetime.getDate()) + '/';
    result += getTwoDigit(datetime.getMonth() + 1) + '/';
    result += datetime.getFullYear();
    return result;
}

function getTimeString(datetime) {
    let result = '';
    result += getTwoDigit(datetime.getHours()) + ':';
    result += getTwoDigit(datetime.getMinutes());
    return result;
}

function getTwoDigit(number) {
    if (number < 10) {
        return '0' + number;
    }
    else return '' + number;
}