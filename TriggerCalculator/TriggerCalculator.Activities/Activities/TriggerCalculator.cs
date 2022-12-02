using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using TriggerCalculator.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace TriggerCalculator.Activities
{
    [LocalizedDisplayName(nameof(Resources.TriggerCalculator_DisplayName))]
    [LocalizedDescription(nameof(Resources.TriggerCalculator_Description))]
    public class TriggerCalculator : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.TriggerCalculator_Duration_DisplayName))]
        [LocalizedDescription(nameof(Resources.TriggerCalculator_Duration_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<Int32> Duration { get; set; }

        [LocalizedDisplayName(nameof(Resources.TriggerCalculator_DurationType_DisplayName))]
        [LocalizedDescription(nameof(Resources.TriggerCalculator_DurationType_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<DurationTypes> DurationType { get; set; }

        [LocalizedDisplayName(nameof(Resources.TriggerCalculator_InitialDate_DisplayName))]
        [LocalizedDescription(nameof(Resources.TriggerCalculator_InitialDate_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<DateTime> InitialDate { get; set; }

        [LocalizedDisplayName(nameof(Resources.TriggerCalculator_TimeZoneId_DisplayName))]
        [LocalizedDescription(nameof(Resources.TriggerCalculator_TimeZoneId_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> TimeZoneId { get; set; }

        [LocalizedDisplayName(nameof(Resources.TriggerCalculator_IsRunnable_DisplayName))]
        [LocalizedDescription(nameof(Resources.TriggerCalculator_IsRunnable_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<bool> IsRunnable { get; set; }

        #endregion


        #region Constructors

        public TriggerCalculator()
        {
        }

        #endregion




        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Duration == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Duration)));
            if (InitialDate == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InitialDate)));
            if (DurationType == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DurationType)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var duration = Duration.Get(context);
            var durationtype = DurationType.Get(context);
            var initialdate = InitialDate.Get(context);
            var timezoneid = TimeZoneId.Get(context);

            var isRunnable = triggerCalculator(initialdate, durationtype, timezoneid, duration);

            // Outputs
            return (ctx) => {
                IsRunnable.Set(ctx, isRunnable);
            };
        }

        #endregion

        public bool triggerCalculator(DateTime initialDate, DurationTypes durationType, string zone, int counter)
        {
            DateTime date = DateTime.Now;
            Boolean triggerFlag = false;

            if (zone != null)
            {
                TimeZoneInfo timeZoneInfo;

                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);
                }
                catch (Exception ex)
                {
                    timeZoneInfo = TimeZoneInfo.Local;
                }
                initialDate = TimeZoneInfo.ConvertTime(initialDate, timeZoneInfo);
                date = TimeZoneInfo.ConvertTime(date, timeZoneInfo);
            }

            while (date > initialDate)
            {
                switch (durationType)
                {

                    case DurationTypes.Year:
                        {
                            initialDate = initialDate.AddYears(counter);
                            break;
                        }

                    case DurationTypes.Month:
                        {
                            initialDate = initialDate.AddMonths(counter);
                            break;
                        }
                    case DurationTypes.Day:
                        {
                            initialDate = initialDate.AddDays(counter);
                            break;
                        }
                    case DurationTypes.Hour:
                        {
                            initialDate = initialDate.AddHours(counter);
                            break;
                        }
                    case DurationTypes.Minute:
                        {
                            initialDate = initialDate.AddMinutes(counter);
                            break;
                        }
                }
            }

            counter = counter * (-1);

            switch (durationType)
            {

                case DurationTypes.Year:
                    {
                        initialDate = initialDate.AddYears(counter);
                        break;
                    }

                case DurationTypes.Month:
                    {
                        initialDate = initialDate.AddMonths(counter);
                        break;
                    }
                case DurationTypes.Day:
                    {
                        initialDate = initialDate.AddDays(counter);
                        break;
                    }
                case DurationTypes.Hour:
                    {
                        initialDate = initialDate.AddHours(counter);
                        break;
                    }
                case DurationTypes.Minute:
                    {
                        initialDate = initialDate.AddMinutes(counter);
                        break;
                    }
            }

            if (date.Subtract(initialDate).TotalSeconds <= 60)
                triggerFlag = true;

            return triggerFlag;
        }

    }
}

