using System;

class ChairReservationLogic
{
        private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();

        public ChairReservationLogic()
        {
            _chairReservation = ChairReservationAccess.LoadAll();
        }
}