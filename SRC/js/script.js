document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('loginForm').addEventListener('submit', async function (event) {
        event.preventDefault();

        const email = document.getElementById('email').value;
        const password = document.getElementById('senha').value;

        const response = await fetch('http://localhost:5284/users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });

        if (response.ok) {
            const data = await response.json();

            const tokenInput = document.getElementById("token");
            tokenInput.value = data.token;

            const tokenInputContainer = document.getElementById("token-input");
            tokenInputContainer.style.display = "block";
        } else {
            alert('Falha ao fazer login. Por favor, verifique seu email e senha.');
        }
    });
});
