export function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);

    if (parts.length === 2) {
        const cookieValue = parts.pop().split(';').shift();
        deleteCookie(name);
        return cookieValue;
    }

    return null;
}

export function deleteCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999; path=/;';
}