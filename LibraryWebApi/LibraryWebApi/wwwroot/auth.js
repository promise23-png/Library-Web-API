const API_BASE_URL = 'https://localhost:7220';
let jwtToken = localStorage.getItem('jwtToken') || null;

// Check if already authenticated
if (jwtToken) {
    document.getElementById('token-section').classList.remove('hidden');
    updateTokenDisplay();
}

function showLoading(message) {
    const loader = document.getElementById('loading-indicator');
    loader.querySelector('div').textContent = message;
    loader.classList.remove('hidden');
}

function hideLoading() {
    document.getElementById('loading-indicator').classList.add('hidden');
}

function showAlert(message, type = 'info') {
    alert(message); // In a real app, replace with a prettier notification system
}

function updateTokenDisplay() {
    const tokenDisplay = document.getElementById('current-token');
    if (jwtToken) {
        tokenDisplay.textContent = jwtToken.substring(0, 30) + '...';
    } else {
        tokenDisplay.textContent = 'None';
    }
}

async function registerUser() {
    const username = document.getElementById('regUsername').value;
    const password = document.getElementById('regPassword').value;
    const email = document.getElementById('regEmail').value;

    if (!username || !password || !email) {
        showAlert('Please fill all fields', 'error');
        return;
    }

    try {
        showLoading('Registering...');
        const response = await fetch(`${API_BASE_URL}/api/Auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password, email })
        });

        const data = await response.json();

        if (response.ok) {
            showAlert('Registration successful! Please login.', 'success');
      
            document.getElementById('regUsername').value = '';
            document.getElementById('regPassword').value = '';
            document.getElementById('regEmail').value = '';
        } else {
            throw new Error(data.message || 'Registration failed');
        }
    } catch (error) {
        showAlert(error.message, 'error');
    } finally {
        hideLoading();
    }
}

async function loginUser() {
    const username = document.getElementById('loginUsername').value;
    const password = document.getElementById('loginPassword').value;

    if (!username || !password) {
        showAlert('Please enter both username and password', 'error');
        return;
    }

    try {
        showLoading('Authenticating...');
        const response = await fetch(`${API_BASE_URL}/api/Auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const data = await response.json();

        if (response.ok && data.token) {
            jwtToken = data.token;
            localStorage.setItem('jwtToken', jwtToken);
            document.getElementById('token-section').classList.remove('hidden');
            updateTokenDisplay();
            showAlert('Login successful!', 'success');
        } else {
            throw new Error(data.message || 'Login failed');
        }
    } catch (error) {
        showAlert(error.message, 'error');
    } finally {
        hideLoading();
    }
}

function copyToken() {
    if (!jwtToken) {
        showAlert('No token available', 'error');
        return;
    }

    navigator.clipboard.writeText(jwtToken).then(() => {
        showAlert('Token copied to clipboard!', 'success');
    }).catch(err => {
        console.error('Failed to copy token:', err);
        showAlert('Failed to copy token', 'error');
    });
}

function proceedToDashboard() {
    if (!jwtToken) {
        showAlert('Please login first', 'error');
        return;
    }
    window.location.href = 'dashboard.html';
}