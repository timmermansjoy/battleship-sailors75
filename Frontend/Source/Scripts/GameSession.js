/*****************************************************************/
/* Grid Coordinate Map                                           */
/*****************************************************************/
var segmentCoordinateMap = new Map();
/*****************************************************************/
/* Grid Coordinate definition                                    */
/*****************************************************************/
class Coordinate {
	constructor(row, col) {
		// Constructor
		this.row = row;
		this.column = col;
	}
}
/*****************************************************************/
/* Starts the page load                                          */
/*****************************************************************/
function OnWindowLoad() {
	console.clear();

	//get the login and signup buttons and allocate to a constant
	let PositionShipBtn = document.getElementById("btnPositionShip");

	//add class slide-up to login button when clicked so the animation in css gets triggerd
	PositionShipBtn.addEventListener("click", (e) => {
		PositionShip();
	});
	//get the login and signup buttons and allocate to a constant
	let StartGameBtn = document.getElementById("btnStartGame");

	//add class slide-up to login button when clicked so the animation in css gets triggerd
	StartGameBtn.addEventListener("click", (e) => {
		StartGame();
	});

	// clear ship info of opponent
	let opponentShipInfo = document.getElementById("opponent-ship-info");
	opponentShipInfo.innerHTML = "";

	// clear own ship info
	let ownShipInfo = document.getElementById("own-ship-info");
	ownShipInfo.innerHTML = "";

	// clear game info
	let output = document.getElementById("game-info");
	output.textContent = "";

	ClearAllCells(0, 10, "lightgrey");

	let shot_count_player = document.getElementById("CNT_PLAYER");
	shot_count_player.textContent = "0";
	let shot_count_opponent = document.getElementById("CNT_OPPONENT");
	shot_count_opponent.textContent = "0";
}
/*****************************************************************/
/* Starts the game                                               */
/*****************************************************************/
function StartGame() {
	let output = document.getElementById("game-info");
	let token = sessionStorage.getItem("token");
	let gameId = sessionStorage.getItem("gameid");
	let url = "https://localhost:5001/api/games/" + gameId;
	url += "/start";
	output.textContent = "";
	fetch(url, {
		method: "POST",
		headers: {
			Accept: "application/json",
			"Content-Type": "application/json",
			Authorization: "Bearer " + token,
		},
	})
		.then((response) => {
			return response.json();
		})
		.then((data) => {
			console.log(data);
			if (data !== undefined && data.isFailure === true) {
				output.textContent = data.message;
			}
		})
		.catch((error) => {
			console.log(error);
		});
}
/*****************************************************************/
/* Places a ship on the grid of player                           */
/*****************************************************************/
function PositionShip() {
	let output = document.getElementById("game-info");
	let token = sessionStorage.getItem("token");
	let gameId = sessionStorage.getItem("gameid");
	let url = "https://localhost:5001/api/games/" + gameId;
	url += "/positionship";
	output.textContent = "";

	let shipCode = "";
	if (segmentCoordinateMap.size === 2) {
		shipCode = "PB";
	} else if (segmentCoordinateMap.size === 3) {
		if (document.getElementById("btnSelectSubmarine").checked === true) {
			shipCode = "SM";
		} else {
			shipCode = "DS";
		}
	} else if (segmentCoordinateMap.size === 4) {
		shipCode = "BS";
	} else if (segmentCoordinateMap.size === 5) {
		shipCode = "CAR";
	} else {
		output.textContent = "Invalid ship length";
		return;
	}

	let segmentCoordinatesArray = [];

	for (let [key, value] of segmentCoordinateMap) {
		segmentCoordinatesArray.push(new Coordinate(value.row, value.column));
	}

	segmentCoordinatesArray.sort(function (a, b) {
		let v1 = a.row * 10 + a.column;
		let v2 = b.row * 10 + b.column;
		if (v1 > v2) {
			return -1;
		} else if (v1 < v2) {
			return 1;
		} else {
			return 0;
		}
	});

	let shipPosition = {
		shipCode: shipCode,
		segmentCoordinates: segmentCoordinatesArray,
	};

	fetch(url, {
		method: "POST",
		headers: {
			Accept: "application/json",
			"Content-Type": "application/json",
			Authorization: "Bearer " + token,
		},
		body: JSON.stringify(shipPosition),
	})
		.then((response) => {
			if (response.status === 200) {
				return response.json();
			} else {
				output.textContent = "No HTTP 200 response from server";
			}
		})
		.then((data) => {
			segmentCoordinateMap.clear();
			if (data.isFailure !== undefined && data.isFailure === true) {
				output.textContent = data.message;
			}
			UpdateOwnGridFromServerAfterPlacementOfShip(false);
		})
		.catch((error) => {
			console.log(error);
		});
}
/*****************************************************************/
/* updates the grid of player when a ship is placed              */
/*****************************************************************/
function UpdateOwnGridFromServerAfterPlacementOfShip(skipStartBtnUpdate) {
	let token = sessionStorage.getItem("token");
	let gameId = sessionStorage.getItem("gameid");
	let url = "https://localhost:5001/api/games/" + gameId;

	fetch(url, {
		method: "GET",
		headers: {
			Accept: "application/json",
			Authorization: "Bearer " + token,
		},
	})
		.then((response) => {
			if (response.status === 200) {
				return response.json();
			} else {
				output.textContent = "No HTTP 200 response from server";
			}
		})
		.then((data) => {
			ClearAllCells(0, data.ownGrid.squares.length, "lightgrey");
			let nrShips = 0;
			for (let ship of data.ownShips) {
				if (ship.coordinates != null) {
					for (let i = 0; i < ship.coordinates.length; i++) {
						DrawSelectedCell(ship.coordinates[i].row, ship.coordinates[i].column, 0, "green");
					}
					nrShips++;
				}
			}
			if (skipStartBtnUpdate == false) {
				if (nrShips === data.ownShips.length) {
					document.getElementById("btnStartGame").style.visibility = "visible";
				} else {
					document.getElementById("btnStartGame").style.visibility = "hidden";
				}
			}
		})
		.catch((error) => {
			console.log(error);
		});
}
/*****************************************************************/
/* shoot at the grid of the opponent                             */
/*****************************************************************/
function ShootShip(row, col) {
	let opponentShipInfo = document.getElementById("opponent-ship-info");
	let shot_count_player = document.getElementById("CNT_PLAYER");

	let output = document.getElementById("game-info");
	let token = sessionStorage.getItem("token");
	let gameId = sessionStorage.getItem("gameid");
	let url = "https://localhost:5001/api/games/" + gameId;
	url += "/shoot";
	output.textContent = "";
	let c = new Coordinate(row, col);
	fetch(url, {
		method: "POST",
		headers: {
			Accept: "application/json",
			"Content-Type": "application/json",
			Authorization: "Bearer " + token,
		},
		body: JSON.stringify(c),
	})
		.then((response) => {
			return response.json();
		})
		.then((data) => {
			if (data !== undefined && data.shotFired !== undefined && data.shotFired === true && data.hit === false) {
				DrawSelectedCell(row, col, 1, "blue");
				shot_count_player.textContent = parseInt(shot_count_player.textContent) + 1;
			} else if (
				data !== undefined &&
				data.shotFired !== undefined &&
				data.shotFired === true &&
				data.hit === true
			) {
				DrawSelectedCell(row, col, 1, "red");
				shot_count_player.textContent = parseInt(shot_count_player.textContent) + 1;
				if (
					data.sunkenShip !== undefined &&
					data.sunkenShip !== null &&
					data.sunkenShip.kind !== undefined &&
					data.sunkenShip.kind !== null
				) {
					if (opponentShipInfo.textContent.includes(data.sunkenShip.kind.code) === false) {
						opponentShipInfo.innerHTML += data.sunkenShip.kind.code + " sunk<br>";
					}
				}
			} else if (data !== undefined && data.shotFired !== undefined && data.shotFired === false) {
				output.textContent = data.misfireReason;
			} else if (data !== undefined && data.applicationException !== undefined) {
				output.textContent = data.applicationException[0];
			}
			UpdateOwnGridFromServerAfterShot();
		})
		.catch((error) => {
			console.log(error);
		});
}
/*****************************************************************/
/* updates the grid of the player when the computer fires a shot */
/*****************************************************************/
function UpdateOwnGridFromServerAfterShot() {
	let token = sessionStorage.getItem("token");
	let gameId = sessionStorage.getItem("gameid");
	let url = "https://localhost:5001/api/games/" + gameId;
	let ownShipInfo = document.getElementById("own-ship-info");
	let shot_count_opponent = document.getElementById("CNT_OPPONENT");
	let shots = 0;

	fetch(url, {
		method: "GET",
		headers: {
			Accept: "application/json",
			Authorization: "Bearer " + token,
		},
	})
		.then((response) => {
			if (response.status === 200) {
				return response.json();
			} else {
				output.textContent = "No HTTP 200 response from server";
			}
		})
		.then((data) => {
			if (data.ownGrid !== undefined && data.ownGrid.squares !== undefined) {
				for (let i = 0; i < data.ownGrid.squares.length; i++) {
					for (let j = 0; j < data.ownGrid.squares[0].length; j++) {
						if (data.ownGrid.squares[i][j].status === 1) {
							//missed shot
							DrawSelectedCell(i, j, 0, "blue");
							shots++;
						} else if (data.ownGrid.squares[i][j].status === 2) {
							//hit shot
							DrawSelectedCell(i, j, 0, "red");
							shots++;
						}
					}
				}
			}
			shot_count_opponent.textContent = shots;
			ListOwnSunkenShips(data, ownShipInfo);
		})
		.catch((error) => {
			console.log(error);
		});
}

/*****************************************************************/
/* selects a cell on the grid of the player                      */
/*****************************************************************/
function SelectCell(row, col) {
	if (document.getElementById("btnPositionShip").style.visibility === "visible") {
		cellid = row.toString() + col.toString();
		celltd = document.getElementById(cellid);
		if (celltd.style.backgroundColor === "red") {
			celltd.style.backgroundColor = "lightgrey";
			segmentCoordinateMap.delete(cellid);
		} else {
			let c = new Coordinate(row, col);
			celltd.style.backgroundColor = "red";
			segmentCoordinateMap.set(cellid, c);
		}
	}
}
/*****************************************************************/
/* clears all cells on the grid of the player                    */
/*****************************************************************/
function ClearAllCells(grid, size, color) {
	for (let i = 0; i < size; i++) {
		for (let j = 0; j < size; j++) {
			DrawSelectedCell(i, j, grid, color);
		}
	}
}
/*****************************************************************/
/* color a cell on the grid of the player or computer            */
/*****************************************************************/
function DrawSelectedCell(row, col, grid, color) {
	if (grid === 1) {
		/* computer grid */
		row += 10;
	}
	celltd = document.getElementById(row.toString() + col.toString());
	celltd.style.backgroundColor = color;
}
/*****************************************************************/
/* list all sunken ships of player                               */
/*****************************************************************/
function ListOwnSunkenShips(data, element) {
	let nrOfOwnSunkenShips = 0;
	let nrOfOpponentSunkenShips = 0;
	let playerMessage = "";

	if (data.ownShips !== undefined) {
		for (let ship of data.ownShips) {
			if (ship.hasSunk === true) {
				nrOfOwnSunkenShips = nrOfOwnSunkenShips + 1;
				playerMessage = playerMessage + ship.kind.code + " sunk<br>";
			}
		}
		element.innerHTML = playerMessage;
	}
	if (data.sunkenOpponentShips !== undefined) {
		nrOfOpponentSunkenShips = data.sunkenOpponentShips.length;
	}

	if (nrOfOwnSunkenShips === 5) {
		ComputerWon();
	} else if (nrOfOpponentSunkenShips === 5) {
		PlayerWon();
	}
	console.log("own sunken ships =" + nrOfOwnSunkenShips);
	console.log("opponent sunken ships =" + nrOfOpponentSunkenShips);
}
/*****************************************************************/
/* End game: computer won                                        */
/*****************************************************************/
function ComputerWon() {
	document.getElementById("Computer-Field").style.visibility = "hidden";
	document.getElementById("Player-Field").style.visibility = "hidden";
	document.getElementById("player-shots").style.visibility = "hidden";
	document.getElementById("end-screen").style.visibility = "visible";
	document.getElementById("end-screen").style.width = "100%";
	document.getElementById("own-ship-info").style.fontSize = "0px";
	document.getElementById("opponent-ship-info").style.fontSize = "0px";
	document.getElementById("victory").textContent = "YOU LOSE!!";
}
/*****************************************************************/
/* End game: player won                                          */
/*****************************************************************/
function PlayerWon() {
	document.getElementById("Computer-Field").style.visibility = "hidden";
	document.getElementById("Player-Field").style.visibility = "hidden";
	document.getElementById("player-shots").style.visibility = "hidden";
	document.getElementById("end-screen").style.visibility = "visible";
	document.getElementById("end-screen").style.width = "100%";
	document.getElementById("own-ship-info").style.fontSize = "0px";
	document.getElementById("opponent-ship-info").style.fontSize = "0px";
	document.getElementById("victory").textContent = "YOU WON!!";
}

/*****************************************************************/
/* Start game                                                    */
/*****************************************************************/
function startGame() {
	UpdateOwnGridFromServerAfterPlacementOfShip(true);
	document.getElementById("Computer-Field").style.visibility = "visible";
	document.getElementById("gameControls").style.visibility = "hidden";
	document.getElementById("player-shots").style.visibility = "visible";
	document.getElementById("gameControls").style.width = "0px";
	document.getElementById("btnPositionShip").style.visibility = "hidden";
	document.getElementById("gameControls").style.visibility = "hidden";
	document.getElementById("btnStartGame").style.visibility = "hidden";
	document.getElementById("btnQuitGame").style.visibility = "hidden";
	ClearAllCells(1, 10, "lightgrey");
}
