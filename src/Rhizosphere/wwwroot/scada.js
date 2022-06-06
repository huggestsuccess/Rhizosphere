let _statusTimeout;

const FETCH_INTERVAL = 1000;

export async function init() {
    _statusTimeout = setTimeout(statusFetch, FETCH_INTERVAL);
}

async function statusFetch() {
    try {
        const status = await getStatus();

        if (status)
            await drawStatus(status);

    } catch (error) {
        console.log(error);
    }
    setTimeout(statusFetch, FETCH_INTERVAL);
}

async function getStatus() {
    const res = await fetch("/Status");

    if (!res.ok)
        throw res;

    return await res.json();
}

async function drawStatus(status) {
    drawElement("temperature",status.temperatureCelsius, "", " C°");
    drawElement("humidity",status.humidityPercentage, "", " %");


    drawElement("mode",status.mode, "", "");
}

function drawElement(id, status, prefix, suffix)
{
    let elem = document.getElementById(id);

    if (id)
        id.textContent = prefix + status + suffix;
}
