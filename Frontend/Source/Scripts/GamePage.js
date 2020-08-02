/*****************************************************************/
/* Starts the page load                                          */
/*****************************************************************/
function OnWindowLoad() {
    console.clear();

    //get the login and signup buttons and allocate to a constant
    let startGameBtn = document.getElementById("btnStartGame");

    //add class slide-up to login button when clicked so the animation in css gets triggerd
    startGameBtn.addEventListener("click", (e) => {
        StartGame();
    });
}
/*****************************************************************/
/* start a game                                                  */
/*****************************************************************/
function StartGame() {
    let url = "https://localhost:5001/api/games";
    let allowDeformedShips = document.getElementById("btnSelectBoatplacement").checked;
    let game =
    {
        gridSize: 10,
        allowDeformedShips: allowDeformedShips,
        mustReportSunkenShip: true,
        canMoveUndamagedShipsDuringGame: false,
        numberOfTurnsBeforeAShipCanBeMoved: 1
    }
    let token = sessionStorage.getItem("token");

    fetch(url, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(game),
    })
        .then((response) => {
            if (response.status == 201 || response.status == 403) {
                return response.json();
            } else {
                output.textContent = "Unknown error";
            }
        })
        .then((data) => {
            console.log(data);
            if (data.id != undefined) {
                sessionStorage.setItem("gameid", data.id);
                window.location.href = "GameWindow.html";
            }
        })
        .catch((error) => {
            console.log(error);
        });
}
