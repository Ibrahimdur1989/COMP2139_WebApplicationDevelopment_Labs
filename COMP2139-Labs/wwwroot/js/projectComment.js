function loadComments(projectId) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: 'GET',
        success: function (data) {
            var commentHtml = '';
            for (var i = 0; i < data.length; i++) {
                commentHtml += '<div class="comment">';
                commentHtml += '<p>' + data[i].content + '</p>';
                commentHtml += '<span>Posted On: ' + new Date(data[i].DatePosted).toLocaleDateString() + '</span>';
                commentHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        }
        
        
    });
    
    
    
    
    
}