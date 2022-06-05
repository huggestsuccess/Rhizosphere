import * as Scada from "./scada.js";


async function main(){
    await Scada.init();
}

window.addEventListener('load', main);