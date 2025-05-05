export default class MultiStepModal {
    constructor(modalSelector) {
        this.$modal = $(modalSelector);
        this.steps = this.$modal.find('.step');
        this.currentStep = 1;
        this.totalSteps = this.steps.length;

        this.$prevBtn = this.$modal.find('#prevBtn');
        this.$nextBtn = this.$modal.find('#nextBtn');
        this.$finishBtn = this.$modal.find('#finishBtn');
        this.$modalTitle = this.$modal.find('.modal-title');

        this.bindEvents();
    }

    bindEvents() {
        this.$modal.on('show.bs.modal', () => this.showStep(this.currentStep));

        this.$nextBtn.on('click', () => {
            if (this.currentStep < this.totalSteps) {
                this.currentStep++;
                this.showStep(this.currentStep);
            }
        });

        this.$prevBtn.on('click', () => {
            if (this.currentStep > 1) {
                this.currentStep--;
                this.showStep(this.currentStep);
            }
        });

        this.$finishBtn.on('click', () => {
            this.$modal.find('form').submit();
        });
    }

    showStep(step) {
        this.steps.hide();
        const $current = this.$modal.find(`#step${step}`).show();
        const title = $current.data('title');

        this.$modalTitle.text(title);

        this.$prevBtn.toggle(step > 1);
        this.$nextBtn.toggle(step < this.totalSteps);
        this.$finishBtn.toggle(step === this.totalSteps);

        const progress = (step / this.totalSteps) * 100;
        this.$modal.find('#step-progress').css('width', `${progress}%`);
    }
}
