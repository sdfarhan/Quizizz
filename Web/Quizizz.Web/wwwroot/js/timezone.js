$(function () {
    const timezon = Intl.DateTimeFormat().resolvedOptions().timeZone;
    $('#timezone').val(timezone);
})