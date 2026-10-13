
$(document).ready(function () {
    $('#productTable').DataTable({
        searching: true,
        paging: true,
        ordering: false,
        pageLength: 5,
        language: {
            search: "Search national parks:",
            searchPlaceholder: "Enter park name or state..."
        }
    });
});
