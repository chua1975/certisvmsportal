var calendar;

document.addEventListener('DOMContentLoaded', function () {
    var Calendar = FullCalendar.Calendar;
    var Draggable = FullCalendarInteraction.Draggable;
    var containerEl = document.getElementById('external-events');
    new Draggable(containerEl, {
        itemSelector: '.fc-event',
        eventData: function (eventEl) {
            return {
                title: eventEl.innerText.trim()
            }
        }
    });



    var today = moment(new Date()).format("YYYY-MM-DD");
    var calendarEl = document.getElementById('calendar');
    calendar = new Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid', 'timeGrid', 'resourceTimeline'],
        now: today,
        editable: true,
        droppable: true,
        aspectRatio: 1.8,
        scrollTime: '00:00',
        resourceAreaWidth: 100,
        header: false,
        height: 400,
        defaultView: 'resourceTimelineDay',
        resourceLabelText: 'Days',
        resources: [
            { id: 'Sun', title: 'Sunday' },
            { id: 'Mon', title: 'Monday', eventColor: 'orange' },
            { id: 'Tue', title: 'Tuesday', eventColor: 'green' },
            { id: 'Wed', title: 'Wednesday', eventColor: 'green' },
            { id: 'Thu', title: 'Thursday', eventColor: 'green' },
            { id: 'Fri', title: 'Friday', eventColor: 'green' },
            { id: 'Sat', title: 'Saturday', eventColor: 'orange' },
            { id: 'PH', title: 'Public Holiday', eventColor: 'red' }
        ],
        events: eventList,
        drop: function (arg) {
            //debugger;
        },
        eventReceive: function (arg) { // called when a proper external event is dropped
            //console.log('eventReceive', arg.event);
        },
        eventDrop: function (arg) { // called when an event (already on the calendar) is moved
            //console.log('eventDrop', arg.event);
        },
        eventClick: function (arg) {
            $('#btnDelete').unbind('click');
            $("#btnDelete").on("click", function () {
                $('#eventPopup').on('hidden.bs.modal', function () {
                    arg.event.remove();
                    $('#eventPopup').unbind();
                });
            });
            $('#eventPopup').modal('show');
        }
    });
    calendar.render();

    $(".fc-license-message").css('display', 'none');
});


function getEventsToSave() {
    var events = calendar.getEvents();
    var eventList = [];
    var AllowedDayOfWeek = "";
    for (var i = 0; i < events.length; i++) {
        var start = moment(events[i].start).format("HH:mm");
        var end = moment(events[i].end).format("HH:mm");
        if (events[i].end == null) {
            end = moment(events[i].start).add(1, 'hours').format("HH:mm");
        }
        var resourceId = events[i].getResources().map(function (resource) { return resource.id });

        if (end == "00:00") {
            end = "24:00";
        }

        var paraData = { "Day": resourceId[0], "Range": [start + "-" + end] };
        var isNew = true;
        for (x in eventList) {
            debugger;
            if (eventList[x].Day == resourceId[0]) {
                isNew = false;
                eventList[x].Range.push(start + "-" + end);
            }
        }
        if (isNew) {
            eventList.push(paraData);
            if (resourceId[0] == "Sun") { AllowedDayOfWeek += "0,"; }
            else if (resourceId[0] == "Mon") { AllowedDayOfWeek += "1,"; }
            else if (resourceId[0] == "Tue") { AllowedDayOfWeek += "2,"; }
            else if (resourceId[0] == "Wed") { AllowedDayOfWeek += "3,"; }
            else if (resourceId[0] == "Thu") { AllowedDayOfWeek += "4,"; }
            else if (resourceId[0] == "Fri") { AllowedDayOfWeek += "5,"; }
            else if (resourceId[0] == "Sat") { AllowedDayOfWeek += "6,"; }
            else if (resourceId[0] == "PH") { AllowedDayOfWeek += "7,"; }
        }
    }

    var timeRange = JSON.stringify(eventList);
    $("#EntrySchedule").val(JSON.stringify({
        "TypeId": typeID,
        "DorsconId": DorsconID,
        "TimeRange": timeRange,
        "AllowEntryOnPublicHoliday": $("#AllowEntryOnPublicHoliday").prop("checked"),
        "AllowedDayOfWeek": AllowedDayOfWeek
    }));

    return true;
}
