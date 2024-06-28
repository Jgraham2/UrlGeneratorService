const apiBaseUrl = 'https://localhost:7281/api/UrlShortener';

function isValidUrl(url) {
    try {
        new URL(url);
        return true;
    } catch (_) {
        return false;
    }
}

async function shortenUrl() {
    const originalUrl = document.getElementById('originalUrl').value;
    const resultDiv = document.getElementById('result');
    const errorDiv = document.getElementById('error');
    resultDiv.innerText = '';
    errorDiv.innerText = '';

    if (!isValidUrl(originalUrl) || originalUrl.includes('localhost')) {
        errorDiv.innerText = 'Needs to be a real URL';
        return;
    }

    try {
        const response = await fetch(apiBaseUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(originalUrl)
        });

        if (!response.ok) {
            const error = await response.json();
            errorDiv.innerText = error.message;
            return;
        }

        const data = await response.json();
        resultDiv.innerText = `Short URL: ${data.shortUrl}`;
    } catch (error) {
        errorDiv.innerText = 'An error occurred while shortening the URL';
    }
}

async function getOriginalUrl() {
    const shortUrl = document.getElementById('shortUrl').value;
    const resultDiv = document.getElementById('result');
    const errorDiv = document.getElementById('error');
    resultDiv.innerText = '';
    errorDiv.innerText = '';

    try {
        const response = await fetch(`${apiBaseUrl}?shortUrl=${encodeURIComponent(shortUrl)}`);

        if (!response.ok) {
            const error = await response.json();
            errorDiv.innerText = error.message;
            return;
        }

        const data = await response.json();
        resultDiv.innerText = `Original URL: ${data.originalUrl}`;
    } catch (error) {
        errorDiv.innerText = 'An error occurred while retrieving the original URL';
    }
}

async function redirectToOriginal() {
    const redirectUrl = document.getElementById('redirectUrl').value;
    const errorDiv = document.getElementById('error');
    errorDiv.innerText = '';

    if (!isValidUrl(redirectUrl)) {
        errorDiv.innerText = 'Invalid short URL';
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/redirect?shortUrl=${encodeURIComponent(redirectUrl)}`);

        if (!response.ok) {
            const error = await response.json();
            errorDiv.innerText = error.message;
            return;
        }

        const data = await response.json();
        window.open(data.originalUrl, '_blank');
    } catch (error) {
        errorDiv.innerText = 'An error occurred while redirecting to the original URL';
    }
}
