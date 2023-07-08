$(document).ready(function () {
    $(".task").draggable({
        revert: "invalid",
        helper: "clone",
        cursor: "move"
    });

    $(".column").droppable({
        accept: ".task",
        drop: function (event, ui) {
            $(ui.draggable).detach().appendTo($(this));
            updateTaskStatus(ui.draggable.data("task-id"), $(this).data("status"));
        }
    });

    function updateTaskStatus(taskId, status) {
        $.ajax({
            url: '/Task/UpdateTaskStatus',
            type: 'POST',
            data: { taskId: taskId, status: status },
            success: function (response) {
                console.log("Task status updated.");
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
});
