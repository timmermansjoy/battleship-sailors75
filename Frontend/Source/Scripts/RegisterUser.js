/*****************************************************************/
/* user password match                                           */
/*****************************************************************/
matching = () => {
	let mailPattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	let user = document.getElementById("user").value;
	let mail = document.getElementById("email1").value;
	let pass1 = document.getElementById("password1").value;
	let pass2 = document.getElementById("password2").value;
	if (pass1 === pass2 && pass1.length >= 6 && user.length != 0 && mailPattern.test(mail) == true) {
		return true;
		// check if user is given
	} else if (user.length == 0 || user === null) {
		document.getElementById("error").innerHTML = "no username given";
		return false;
		//check if mail is valid
	} else if (mailPattern.test(mail) == false) {
		document.getElementById("error").innerHTML = "not a valid e-mail";
		return false;
		// check if password has desierd length
	} else if (pass1.length < 6 && pass2.length < 6) {
		document.getElementById("error").innerHTML = "password must be 6 characters long";
		return false;
		// only option is passwords dont match
	} else {
		document.getElementById("error").innerHTML = "passwords do not match";
		return false;
	}
};
/*****************************************************************/
/* user registration request                                     */
/*****************************************************************/
function RegisterUser(email, nickName, password) {
	let loginBtn = document.getElementById("login");
	let url = "https://localhost:5001/api/Authentication/register";
	let player = {
		email: email,
		nickName: nickName,
		password: password,
	};
	let output = document.getElementById("error");
	output.textContent = "";

	fetch(url, {
		method: "POST",
		body: JSON.stringify(player),
		headers: {
			"Content-Type": "application/json",
			Accept: "application/json",
		},
	})
		.then((response) => {
			if (response.status == 400) {
				return response.json();
			} else if (response.status == 200) {
				loginBtn.click();
			}
		})
		.then((data) => {
			if (data != undefined && data.DuplicateUserName != undefined) {
				output.innerHTML = data.DuplicateUserName[0];
			}
			else if (data != undefined) {
				output.innerHTML = "Unknown error";
			}
		})
		.catch(error)
		{
			console.log(error);
		}
}
