/*****************************************************************/
/* Starts the page load                                          */
/*****************************************************************/
function OnWindowLoad() {
	console.clear();

	//get the login and signup buttons and allocate to a constant
	let loginBtn = document.getElementById("login");
	let signupBtn = document.getElementById("signup");
	let btnSignUp = document.getElementById("btnSignUp");
	let btnLogIn = document.getElementById("btnLogIn");

	//add class slide-up to login button when clicked so the animation in css gets triggered
	loginBtn.addEventListener("click", (e) => {
		let parent = e.target.parentNode.parentNode;
		Array.from(parent.classList).find((element) => {
			signupBtn.parentNode.classList.add("slide-up");
			parent.classList.remove("slide-up");
		});
		document.getElementById("email2").value=document.getElementById("email1").value;

	});

	//add class slide-up to signup button when clicked so the animation in css gets triggered
	signupBtn.addEventListener("click", (e) => {
		let parent = e.target.parentNode;
		Array.from(parent.classList).find((element) => {
			loginBtn.parentNode.parentNode.classList.add("slide-up");
			parent.classList.remove("slide-up");
		});
	});

	btnSignUp.addEventListener("click", (e) => {
		//preventing default behaviour until password is good
		if (matching() == false) {
			e.preventDefault();
		}
		//send registration to server
		else {
			let nickname = document.getElementById("user").value;
			let email = document.getElementById("email1").value;
			let password = document.getElementById("password1").value;
			RegisterUser(email, nickname, password);
		}
	});

	btnLogIn.addEventListener("click", (e) => {
		let email = document.getElementById("email2").value;
		let password = document.getElementById("password3").value;
		LoginUser(email, password);
	});
}
