// Check authentication on dashboard load
document.addEventListener('DOMContentLoaded', function () {
    const jwtToken = localStorage.getItem('jwtToken');
    if (!jwtToken) {
        window.location.href = 'index.html';
        return;
    }

    // Show token
    document.getElementById('current-token').textContent = jwtToken.substring(0, 30) + '...';

    // Show books section by default
    showSection('books-section');
});

function showSection(sectionId) {
    // Hide all sections
    document.querySelectorAll('.section').forEach(section => {
        section.classList.add('hidden');
    });

    // Show selected section
    document.getElementById(sectionId).classList.remove('hidden');
}

function copyToken() {
    const jwtToken = localStorage.getItem('jwtToken');
    if (!jwtToken) {
        alert('No token available');
        return;
    }

    navigator.clipboard.writeText(jwtToken).then(() => {
        alert('Token copied to clipboard!');
    }).catch(err => {
        console.error('Failed to copy token:', err);
        alert('Failed to copy token');
    });
}

function logout() {
    localStorage.removeItem('jwtToken');
    window.location.href = 'index.html';
}

// API Functions
async function authenticatedFetch(url, options = {}) {
    const jwtToken = localStorage.getItem('jwtToken');
    if (!jwtToken) {
        alert('Please login first');
        window.location.href = 'index.html';
        throw new Error("Authentication required");
    }

    const headers = {
        'Authorization': `Bearer ${jwtToken}`,
        'Content-Type': 'application/json',
        ...options.headers
    };

    const response = await fetch(url, { ...options, headers });

    if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.message || response.statusText;
        alert(`API Error: ${response.status} - ${errorMessage}`);
        throw new Error(`API Error: ${response.status} ${errorMessage}`);
    }

    return response.json();
}

function displayOutput(data) {
    document.getElementById('output').textContent = JSON.stringify(data, null, 2);
}

// Books API
async function getBooks() {
    try {
        const books = await authenticatedFetch(`${API_BASE_URL}/api/Books`);
        displayOutput(books);
    } catch (error) {
        console.error("Error fetching books:", error);
    }
}

async function addBook() {
    const newBook = {
        title: "Sample Book " + Math.floor(Math.random() * 1000),
        author: "Sample Author",
        isbn: "978" + Math.floor(Math.random() * 1000000000).toString().padStart(10, '0'),
        totalCopies: Math.floor(Math.random() * 10) + 1
    };

    try {
        const result = await authenticatedFetch(`${API_BASE_URL}/api/Books`, {
            method: 'POST',
            body: JSON.stringify(newBook)
        });
        displayOutput(result);
        alert('Book added successfully!');
    } catch (error) {
        console.error("Error adding book:", error);
    }
}

// Borrowers API
async function getBorrowers() {
    try {
        const borrowers = await authenticatedFetch(`${API_BASE_URL}/api/Borrowers`);
        displayOutput(borrowers);
    } catch (error) {
        console.error("Error fetching borrowers:", error);
    }
}

async function addBorrower() {
    const newBorrower = {
        name: "Borrower " + Math.floor(Math.random() * 1000),
        email: `borrower${Math.floor(Math.random() * 1000)}@example.com`,
        phoneNumber: "555-" + Math.floor(Math.random() * 1000).toString().padStart(4, '0')
    };

    try {
        const result = await authenticatedFetch(`${API_BASE_URL}/api/Borrowers`, {
            method: 'POST',
            body: JSON.stringify(newBorrower)
        });
        displayOutput(result);
        alert('Borrower added successfully!');
    } catch (error) {
        console.error("Error adding borrower:", error);
    }
}

// Loans API
async function issueLoan() {
    const loanData = {
        bookId: 1,      // Replace with actual book ID
        borrowerId: 1,  // Replace with actual borrower ID
        dueDate: new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString() // 2 weeks from now
    };

    try {
        const result = await authenticatedFetch(`${API_BASE_URL}/api/Loans`, {
            method: 'POST',
            body: JSON.stringify(loanData)
        });
        displayOutput(result);
        alert('Loan issued successfully!');
    } catch (error) {
        console.error("Error issuing loan:", error);
    }
}

async function returnLoan() {
    const returnData = {
        loanId: 1  // Replace with actual loan ID
    };

    try {
        const result = await authenticatedFetch(`${API_BASE_URL}/api/Loans/returns`, {
            method: 'POST',
            body: JSON.stringify(returnData)
        });
        displayOutput(result);
        alert('Loan returned successfully!');
    } catch (error) {
        console.error("Error returning loan:", error);
    }
}

async function getOverdueLoans() {
    try {
        const overdueLoans = await authenticatedFetch(`${API_BASE_URL}/api/Loans/overdue`);
        displayOutput(overdueLoans);
    } catch (error) {
        console.error("Error fetching overdue loans:", error);
    }
}