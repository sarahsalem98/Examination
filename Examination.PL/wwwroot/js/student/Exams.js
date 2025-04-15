function sortTable(column, asc) {
    const table = document.querySelector('table tbody');
    const rows = Array.from(table.rows);

    // Sorting rows array
    rows.sort((a, b) => {
        const aColText = a.cells[column].textContent.trim();
        const bColText = b.cells[column].textContent.trim();

        // Modify this part if specific data type comparisons are needed (like dates)
        return aColText > bColText ? (asc ? 1 : -1) : (asc ? -1 : 1);
    });

    // Re-adding sorted rows to tbody
    while (table.firstChild) {
        table.removeChild(table.firstChild);
    }
    rows.forEach(row => table.appendChild(row));
}
