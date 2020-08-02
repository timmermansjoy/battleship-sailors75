/*****************************************************************/
/* user login request                                            */
/*****************************************************************/
function LoginUser(email, password) {
    let url = "https://localhost:5001/api/Authentication/token/";
    let player = { email: email, password: password };
    let output = document.getElementById("error2");
    output.textContent = "";

    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Accept: "application/json",
        },
        body: JSON.stringify(player),
    })
        .then((response) => {
            if (response.status == 200 || response.status == 401) {
                return response.json();
            } else if (response.status == 400) {
                output.textContent = "User login failed";
            } else {
                output.textContent = "Unknown error";
            }
        })
        .then((data) => {
            if (data != undefined) {
                if (data.status != undefined && data.status == 401) {
                    output.textContent = data.title;
                } else if (data.token != undefined) {
                    sessionStorage.setItem('token', data.token);
                    console.log(sessionStorage.getItem('token'));
                    output.textContent = "User login successful";
                    window.location.href="Startpagina.html";
                }

            }
        })
        .catch(error)
        {
            console.log(error);
        }
}

