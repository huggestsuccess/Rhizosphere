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
    let temperatureElement = document.getElementById(temperature);

    if (temperatureElement)
        temperatureElement.textContent = status.temperatureCelsius + " C°";


    let humidityElement = document.getElementById(humidity);

    if (humidityElement)
        humidityElement.textContent = status.humidityPercentage + " %";
}