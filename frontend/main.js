
window.addEventListener('DOMContentLoaded', (event) =>{
    getVisitCount();
})
const functionApiUrl = 'https://getresumecountermariam.azurewebsites.net/api/GetResumeCounter?code=Xop9XQ6zgVNwM9LPf9DXQhcokVtahvYzoRaCJxjLE03BAzFugiPObw%3D%3D'
const localFunctionApi = 'http://localhost:7071/api/GetResumeCounter';

const getVisitCount = () => {
    let count = 30;
    fetch(functionApiUrl).then(response => {
        return response.json()
    }).then(response => {
        console.log ("Website called function Api");
        count =response.count;
        document.getElementById("counter").innerText =count;
    }).catch(function(error){
        console.log(error);
    });
    return count;
}

