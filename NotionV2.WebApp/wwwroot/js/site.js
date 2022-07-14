function BindCategoryNavItems() {
    $("a[category-id]").click(function () {
        let item = $(this).closest(".category-navbar-item");
        let id = $(this).attr('category-id');

        $("#searchCategory").val(id);

        $.get("/Home/GetPostsHtml?category=" + id, function(data) {
            $("#postsHtmlBody").html(data);

            $(".category-navbar-item").removeClass("active");
            item.addClass("active");

            let articleNavs = $(".article-navbar-item a[post-id]");
            if (articleNavs.length)
            {
                articleNavs.first().click();
            }
            else
            {
                $.get("/Home/GetPostHtml?post=-1", function(data) {
                    $("#postHtmlBody").html(data);
                });
            }
        });
    });
}

function BindPostNavItems() {
    $("a[post-id]").click(function () {
        let item = $(this).closest(".article-navbar-item");
        let id = $(this).attr('post-id');

        $.get("/Home/GetPostHtml?post=" + id, function(data) {
            $("#postHtmlBody").html(data);

            $(".article-navbar-item").removeClass("active");
            item.addClass("active");
        });
    });
}

function BindSectionControlButtons() {
    $("#sectionCreateButton").click(function () {
        $("#sectionEditDialog input[name='sectionId']").attr("value", "-1");
        $("#sectionEditDialog input[name='sectionTitle']").attr("value", "");
        $("#sectionEditDialog").modal();
    });
    $(".sectionDeleteButton").click(function () {
        let id = $(this).attr('section-id');

        $("#sectionDeleteDialog a").attr("href", "/Home/DeleteCategory?catId=" + id);
        $("#sectionDeleteDialog").modal();
    });
    $(".sectionEditButton").click(function () {
        let id = $(this).attr('section-id');
        let selectedSectionItem = $('div[section-id=' + id + '] a').text();
        // let selectedSectionTitle = selectedSectionItem.find('a').text();

        $("#sectionEditDialog input[name='sectionId']").attr("value", id);
        $("#sectionEditDialog input[name='sectionTitle']").attr("value", selectedSectionItem);
        $("#sectionEditDialog").modal();
    });
}

function BindPostsControlButtons() {
    $("#postCreateButton").click(function () {
        $("#postEditDialog input[name='postId']").attr("value", "-1");
        $("#postEditDialog input[name='postTitle']").attr("value", "");
        $("#postEditDialog textarea[name='postText']").val("");

        $("#postEditDialog").modal();
    });
    $(".postDeleteButton").click(function () {
        let selectedPostId = $(this).attr('post-id');

        $("#postDeleteDialog a").attr("href", "/Home/DeletePost?postId=" + selectedPostId);
        $("#postDeleteDialog").modal();
    });
    $(".postEditButton").click(function () {
        let selectedPostId = $(this).attr('post-id');
        let selectedPostItem = $('div[post-id=' + selectedPostId + ']');
        let selectedPostTitle = selectedPostItem.find('h5 a').text();
        let selectedPostContents = selectedPostItem.find('p').text();

        $("#postEditDialog input[name='postId']").attr("value", selectedPostId);
        $("#postEditDialog input[name='postTitle']").attr("value", selectedPostTitle);
        $("#postEditDialog textarea[name='postText']").val(selectedPostContents);

        $("#postEditDialog").modal();
    });
}