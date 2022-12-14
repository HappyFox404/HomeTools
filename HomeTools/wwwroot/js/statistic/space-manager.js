$(document).ready(() => {
    const diagramFilesData = $('#statistic-space-manage-files-diagram');
    const diagramNoFilesData = $('#statistic-space-manage-no-files-diagram');
    
    if(diagramFilesData.length > 0){
        let files = [];
        $('.file-data').each((idx, elem) => {
            files.push($(elem).attr('data-file'));
        });
        $.ajax({
            url: StatisticRoutes.GetFilesData,
            method: 'post',
            dataType: 'json',
            data: {files}
        }).done((data) => {
            ProcessingServerResponse(data,
                () => {
                    const diagrammCanvas = document.getElementById('statistic-space-manage-files-diagram-canvas');
                    
                    if(diagramNoFilesData.hasClass('is-hidden') === false){
                        diagramNoFilesData.addClass('is-hidden');
                    }
                    if(diagramFilesData.hasClass('is-hidden') === true){
                        diagramFilesData.removeClass('is-hidden');
                    }
                    
                    new Chart(diagrammCanvas, {
                        type: 'pie',
                        data: {
                            labels: data.data.map((item) => item.fileName),
                            datasets: [
                                {
                                    label: 'Данные',
                                    data: data.data.map((item) => item.size)
                                }
                            ]
                        },
                        options: {
                            display: false,
                            responsive: true,
                            plugins: {
                                legend: {
                                    position: 'bottom',
                                    display: (data.data.length <= 10)
                                },
                                title: {
                                    display: true,
                                    text: 'Диаграмма данных'
                                },
                                tooltip: {
                                    callbacks: {
                                        footer: (tooltipItems) => {
                                            const sizes = [ "B", "KB", "MB", "GB", "TB" ];
                                            let len = tooltipItems[0].raw;
                                            let order = 0;
                                            while (len >= 1024 && order < sizes.length - 1) {
                                                order++;
                                                len = len/1024;
                                            }
                                            return `Размер: ${len.toLocaleString('ru-RU', {minimumFractionDigits:2})} ${sizes[order]}`;
                                        },
                                    }
                                }
                            }
                        },
                    });
                }, () => {
                    if(diagramNoFilesData.hasClass('is-hidden') === true){
                        diagramNoFilesData.removeClass('is-hidden');
                    } 
                    if(diagramFilesData.hasClass('is-hidden') === false){
                        diagramFilesData.addClass('is-hidden');
                    }
                });
        });
    }
})