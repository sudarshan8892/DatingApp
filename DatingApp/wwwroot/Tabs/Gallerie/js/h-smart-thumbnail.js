(function ($) {
	$.fn.hSmartThumbnail = function () {
		return this.each(function() {
			$(this).addClass('h-smart-thumbnail-container')
			.find('ul').addClass('h-smart-thumbnail');

			let imageCount = $(this).find('ul li').length;
			$(this).attr('data-image-count', imageCount);

			let index = 0;
			$(this).find('ul li').each(function() {
				$($(this)).attr('data-index', index++)
				.attr('data-src', $($(this)).find('img').attr('src'))
				.addClass('h-pic-view');
			});

			$(this).append(
				'<div class="h-big-pic-view-main">' + 
				'<div class="h-big-pic-view-header">' +
				'<span class="icon icon-rotate-left"></span>' +
				'<span class="icon icon-rotate-right"></span>' +
				'<span class="icon icon-close"></span>' +
				'</div>' +
				'<div class="h-big-pic-view-body">' +
				'<div class="h-big-pic-view-image" data-curr-index="0" data-curr-angle="0">' +
				'</div>' +
				'</div>' +
				'<span class="h-arrow h-next-image icon icon-angle-left"></span>' +
				'<span class="h-arrow h-prev-image icon icon-angle-right"></span>' +
				'</div>'
				);

			$(this).find('ul li').on('click', function(event) {
				event.preventDefault();
				$($(this)).parents('ul').find('li').removeClass('active');
				let imgSrc = $($(this)).find('img').attr('src');
				let indexNum = $($(this)).addClass('active').attr('data-index');
				let bigImgView = $($(this)).parents('ul').siblings('.h-big-pic-view-main');

				$(bigImgView).find('.h-big-pic-view-image').attr('data-curr-index', indexNum);
				$(bigImgView).find('.h-big-pic-view-image').html('<img src="'+imgSrc+'" />');
				$(bigImgView).show();
			});

			$(this).find('.h-big-pic-view-main').hover(function() {
				$($(this)).find('.h-arrow').css('display', 'inline-block');
			}, function() {
				$($(this)).find('.h-arrow').css('display', 'none');
			});

			$(this).find('.h-big-pic-view-main .icon-angle-left').on('click',  function(event) {
				event.preventDefault();
				let currIndex = $($(this)).siblings('.h-big-pic-view-body').find('.h-big-pic-view-image').attr('data-curr-index');
				let totalIndex = $($(this)).parents('.h-smart-thumbnail-container').attr('data-image-count');
				if (parseInt(currIndex) === 0) {
					currIndex = totalIndex-1;
				}else{
					currIndex = parseInt(currIndex) - 1;
				}
				changeImage(currIndex, $($(this)));
			});

			$(this).find('.h-big-pic-view-main .icon-angle-right').on('click',  function(event) {
				event.preventDefault();
				let currIndex = $($(this)).siblings('.h-big-pic-view-body').find('.h-big-pic-view-image').attr('data-curr-index');
				let totalIndex = $($(this)).parents('.h-smart-thumbnail-container').attr('data-image-count');
				if (parseInt(currIndex) === totalIndex-1) {
					currIndex = 0;
				}else{
					currIndex = parseInt(currIndex) + 1;
				}
				changeImage(currIndex, $($(this)));
			});

			$(this).find('.h-big-pic-view-header .icon-close').on('click', function(event) {
				event.preventDefault();
				$($(this)).parents('.h-big-pic-view-main').hide();
				$($(this)).parents('.h-smart-thumbnail-container').find('ul li').removeClass('active');
			});

			$(this).find('.h-big-pic-view-header .icon-rotate-right').on('click', function(event) {
				event.preventDefault();
				let imgView = $($(this)).parents('.h-big-pic-view-main').find('.h-big-pic-view-body .h-big-pic-view-image');
				let currAngle = $(imgView).attr('data-curr-angle');
				if (parseInt(currAngle) === 360) {
					currAngle = 0;
				}else {
					currAngle = parseInt(currAngle) + 90;
				}
				rotateImage(imgView, currAngle);

			});

			$(this).find('.h-big-pic-view-header .icon-rotate-left').on('click', function(event) {
				event.preventDefault();
				let imgView = $($(this)).parents('.h-big-pic-view-main').find('.h-big-pic-view-body .h-big-pic-view-image');
				let currAngle = $(imgView).attr('data-curr-angle');
				if (parseInt(currAngle) === 0) {
					currAngle = 360;
				}else {
					currAngle = parseInt(currAngle) - 90;
				}
				rotateImage(imgView, currAngle);
			});
		});
	}
}(jQuery));

function changeImage(indexNum, ref) {
	$(ref).parents('.h-smart-thumbnail-container').find('ul li[data-index="' + indexNum + '"]').trigger('click');
}

function rotateImage(img, angle) {
	$(img).find('img').removeClass('h-angle-0deg h-angle-90deg h-angle-180deg h-angle-270deg h-angle-360deg')
	.addClass('h-angle-'+angle+'deg');
	$(img).attr('data-curr-angle', angle);
}